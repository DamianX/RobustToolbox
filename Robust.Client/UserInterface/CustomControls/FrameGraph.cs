﻿using Robust.Client.Graphics.Drawing;
using Robust.Shared.Interfaces.Timing;
using Robust.Shared.Maths;

namespace Robust.Client.UserInterface.CustomControls
{
    public sealed class FrameGraph : Control
    {
        private readonly IGameTiming _gameTiming;

        /// <summary>
        ///     How many frames we show at once.
        /// </summary>
        private const int TrackedFrames = 120;

        /// <summary>
        ///     The width of a single frame on the bar chart, in pixels.
        /// </summary>
        private const int FrameWidth = 4;

        /// <summary>
        ///     The height of a single frame in pixels to the target <see cref="TargetFrameRate"/> FPS.
        ///     If the frame takes say 1/30 seconds to complete (30 FPS) it would be twice this value.
        /// </summary>
        private const int FrameHeight = 60;

        /// <summary>
        ///     The target frame rate at which we are "good".
        /// </summary>
        private const int TargetFrameRate = 60;

        // We keep track of frame times in a ring buffer.
        private readonly float[] _frameTimes = new float[TrackedFrames];

        // Position of the last frame in the ring buffer.
        private int _frameIndex;

        public FrameGraph(IGameTiming gameTiming)
        {
            _gameTiming = gameTiming;
        }

        protected override void Initialize()
        {
            base.Initialize();

            SizeFlagsHorizontal = SizeFlags.None;
        }

        protected override Vector2 CalculateMinimumSize()
        {
            return (TrackedFrames * FrameWidth, FrameHeight * 2);
        }

        protected override void FrameUpdate(RenderFrameEventArgs args)
        {
            _frameTimes[_frameIndex] = (float)_gameTiming.RealFrameTime.TotalSeconds;
            _frameIndex = (_frameIndex + 1) % TrackedFrames;
        }

        protected internal override void Draw(DrawingHandleScreen handle)
        {
            base.Draw(handle);

            for (var i = 0; i < TrackedFrames; i++)
            {
                var currentFrameIndex = MathHelper.Mod(_frameIndex - 1 - i, TrackedFrames);
                var frameTime = _frameTimes[currentFrameIndex];
                var x = FrameWidth * UserInterfaceManager.UIScale * (TrackedFrames - 1 - i);
                var frameHeight = FrameHeight * (frameTime / (1f / TargetFrameRate));
                var rect = new UIBox2(x, PixelHeight - frameHeight, x + FrameWidth * UserInterfaceManager.UIScale, PixelHeight);

                Color color;
                if (frameTime > 1f / (TargetFrameRate / 2 - 1))
                {
                    color = Color.Red;
                }
                else if (frameTime > 1f / (TargetFrameRate - 1))
                {
                    color = Color.Yellow;
                }
                else
                {
                    color = Color.Lime;
                }

                handle.DrawRect(rect, color);
            }
        }
    }
}
