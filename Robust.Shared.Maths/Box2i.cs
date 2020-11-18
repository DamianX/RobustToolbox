using System;

namespace Robust.Shared.Maths
{
    [Serializable]
    public struct Box2i : IEquatable<Box2i>
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;

        public readonly Vector2i BottomRight => new Vector2i(Right, Bottom);
        public readonly Vector2i TopLeft => new Vector2i(Left, Top);
        public readonly Vector2i TopRight => new Vector2i(Right, Top);
        public readonly Vector2i BottomLeft => new Vector2i(Left, Bottom);
        public readonly int Width => Math.Abs(Right - Left);
        public readonly int Height => Math.Abs(Top - Bottom);
        public readonly Vector2i Size => new Vector2i(Width, Height);

        public Box2i(Vector2i bottomLeft, Vector2i topRight) : this(topRight.Y, topRight.X, bottomLeft.Y, bottomLeft.X)
        {
        }

        public Box2i(int top, int right, int bottom, int left)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public static Box2i FromDimensions(int left, int bottom, int width, int height)
        {
            return new Box2i(bottom + height, left + width, bottom, left);
        }

        public static Box2i FromDimensions(Vector2i position, Vector2i size)
        {
            return FromDimensions(position.X, position.Y, size.X, size.Y);
        }

        public readonly bool Contains(int x, int y)
        {
            return Contains(new Vector2i(x, y));
        }

        public readonly bool Contains(Vector2i point, bool closedRegion = true)
        {
            var xOk = closedRegion
                ? point.X >= Left ^ point.X > Right
                : point.X > Left ^ point.X >= Right;
            var yOk = closedRegion
                ? point.Y >= Bottom ^ point.Y > Top
                : point.Y > Bottom ^ point.Y >= Top;
            return xOk && yOk;
        }

        /// <summary>Returns a UIBox2 translated by the given amount.</summary>
        public readonly Box2i Translated(Vector2i point)
        {
            return new Box2i(Top + point.Y, Right + point.X, Bottom + point.Y, Left + point.X);
        }

        /// <summary>
        ///     Returns the smallest rectangle that contains both of the rectangles.
        /// </summary>
        public readonly Box2i Union(in Box2i other)
        {
            var left = Math.Min(Left, other.Left);
            var right = Math.Max(Right, other.Right);
            var bottom = Math.Min(Bottom, other.Bottom);
            var top = Math.Max(Top, other.Top);

            if (left <= right && bottom <= top)
                return new Box2i(top, right, bottom, left);

            return new Box2i();
        }

        // override object.Equals
        public override readonly bool Equals(object? obj)
        {
            if (obj is Box2i box)
            {
                return Equals(box);
            }

            return false;
        }

        public readonly bool Equals(Box2i other)
        {
            return other.Left == Left && other.Right == Right && other.Bottom == Bottom && other.Top == Top;
        }

        // override object.GetHashCode
        public override readonly int GetHashCode()
        {
            var code = Left.GetHashCode();
            code = (code * 929) ^ Right.GetHashCode();
            code = (code * 929) ^ Top.GetHashCode();
            code = (code * 929) ^ Bottom.GetHashCode();
            return code;
        }

        public static explicit operator Box2i(Box2 box)
        {
            return new Box2i((int) box.Top, (int) box.Right, (int) box.Bottom, (int) box.Left);
        }

        public static implicit operator Box2(Box2i box)
        {
            return new Box2(box.Top, box.Right, box.Bottom, box.Left);
        }

        public override readonly string ToString()
        {
            return $"({Left}, {Bottom}, {Right}, {Top})";
        }
    }
}
