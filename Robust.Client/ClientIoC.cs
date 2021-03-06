using System;
using Robust.Client.Console;
using Robust.Client.Debugging;
using Robust.Client.GameObjects;
using Robust.Client.GameStates;
using Robust.Client.Graphics;
using Robust.Client.Graphics.ClientEye;
using Robust.Client.Graphics.Clyde;
using Robust.Client.Graphics.Lighting;
using Robust.Client.Graphics.Overlays;
using Robust.Client.Input;
using Robust.Client.Interfaces;
using Robust.Client.Interfaces.Debugging;
using Robust.Client.Interfaces.GameObjects;
using Robust.Client.Interfaces.GameStates;
using Robust.Client.Interfaces.Graphics;
using Robust.Client.Interfaces.Graphics.ClientEye;
using Robust.Client.Interfaces.Graphics.Lighting;
using Robust.Client.Interfaces.Graphics.Overlays;
using Robust.Client.Interfaces.Input;
using Robust.Client.Interfaces.Map;
using Robust.Client.Interfaces.Placement;
using Robust.Client.Interfaces.ResourceManagement;
using Robust.Client.Interfaces.State;
using Robust.Client.Interfaces.UserInterface;
using Robust.Client.Interfaces.Utility;
using Robust.Client.Map;
using Robust.Client.Placement;
using Robust.Client.Player;
using Robust.Client.Reflection;
using Robust.Client.ResourceManagement;
using Robust.Client.State;
using Robust.Client.UserInterface;
using Robust.Client.Utility;
using Robust.Client.ViewVariables;
using Robust.Shared;
using Robust.Shared.GameObjects;
using Robust.Shared.Interfaces.GameObjects;
using Robust.Shared.Interfaces.Map;
using Robust.Shared.Interfaces.Network;
using Robust.Shared.Interfaces.Reflection;
using Robust.Shared.Interfaces.Resources;
using Robust.Shared.IoC;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;

namespace Robust.Client
{
    internal static class ClientIoC
    {
        public static void RegisterIoC(GameController.DisplayMode mode)
        {
            SharedIoC.RegisterIoC();

            IoCManager.Register<IPrototypeManager, PrototypeManager>();
            IoCManager.Register<IEntityManager, ClientEntityManager>();
            IoCManager.Register<IComponentFactory, ClientComponentFactory>();
            IoCManager.Register<ITileDefinitionManager, ClydeTileDefinitionManager>();
            IoCManager.Register<IClydeTileDefinitionManager, ClydeTileDefinitionManager>();
            IoCManager.Register<IGameController, GameController>();
            IoCManager.Register<IGameControllerInternal, GameController>();
            IoCManager.Register<IReflectionManager, ClientReflectionManager>();
            IoCManager.Register<IResourceManager, ResourceCache>();
            IoCManager.Register<IResourceManagerInternal, ResourceCache>();
            IoCManager.Register<IResourceCache, ResourceCache>();
            IoCManager.Register<IResourceCacheInternal, ResourceCache>();
            IoCManager.Register<IClientNetManager, NetManager>();
            IoCManager.Register<IClientEntityManager, ClientEntityManager>();
            IoCManager.Register<IEntityNetworkManager, ClientEntityNetworkManager>();
            IoCManager.Register<IClientGameStateManager, ClientGameStateManager>();
            IoCManager.Register<IBaseClient, BaseClient>();
            IoCManager.Register<IPlayerManager, PlayerManager>();
            IoCManager.Register<IStateManager, StateManager>();
            IoCManager.Register<IUserInterfaceManager, UserInterfaceManager>();
            IoCManager.Register<IUserInterfaceManagerInternal, UserInterfaceManager>();
            IoCManager.Register<IDebugDrawing, DebugDrawing>();
            IoCManager.Register<ILightManager, LightManager>();
            IoCManager.Register<IDiscordRichPresence, DiscordRichPresence>();
            IoCManager.Register<IClientConsole, ClientConsole>();
            IoCManager.Register<IFontManager, FontManager>();
            IoCManager.Register<IFontManagerInternal, FontManager>();
            switch (mode)
            {
                case GameController.DisplayMode.Headless:
                    IoCManager.Register<IClyde, ClydeHeadless>();
                    IoCManager.Register<IClydeAudio, ClydeHeadless>();
                    IoCManager.Register<IClydeInternal, ClydeHeadless>();
                    IoCManager.Register<IInputManager, InputManager>();
                    IoCManager.Register<IFileDialogManager, DummyFileDialogManager>();
                    IoCManager.Register<IUriOpener, UriOpenerDummy>();
                    break;
                case GameController.DisplayMode.Clyde:
                    IoCManager.Register<IClyde, Clyde>();
                    IoCManager.Register<IClydeAudio, Clyde>();
                    IoCManager.Register<IClydeInternal, Clyde>();
                    IoCManager.Register<IInputManager, ClydeInputManager>();
                    IoCManager.Register<IFileDialogManager, FileDialogManager>();
#if LINUX
                    IoCManager.Register<IUriOpener, UriOpenerLinux>();
#elif MACOS
                    IoCManager.Register<IUriOpener, UriOpenerMacOS>();
#elif WINDOWS
                    IoCManager.Register<IUriOpener, UriOpenerWindows>();
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IoCManager.Register<IEyeManager, EyeManager>();

            IoCManager.Register<IPlacementManager, PlacementManager>();
            IoCManager.Register<IOverlayManager, OverlayManager>();
            IoCManager.Register<IOverlayManagerInternal, OverlayManager>();
            IoCManager.Register<IViewVariablesManager, ViewVariablesManager>();
            IoCManager.Register<IViewVariablesManagerInternal, ViewVariablesManager>();

#if LINUX
            IoCManager.Register<IClipboardManager, ClipboardManagerLinux>();
            IoCManager.Register<IClipboardManagerInternal, ClipboardManagerLinux>();
#elif WINDOWS
            IoCManager.Register<IClipboardManager, ClipboardManagerWindows>();
            IoCManager.Register<IClipboardManagerInternal, ClipboardManagerWindows>();
#else
            IoCManager.Register<IClipboardManager, ClipboardManagerUnsupported>();
            IoCManager.Register<IClipboardManagerInternal, ClipboardManagerUnsupported>();
#endif

            IoCManager.Register<ISignalHandler, ClientSignalHandler>();
        }
    }
}
