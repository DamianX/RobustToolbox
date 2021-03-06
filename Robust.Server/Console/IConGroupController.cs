using Robust.Server.Interfaces.Player;

namespace Robust.Server.Console
{
    internal interface IConGroupController
    {
        void Initialize();

        bool CanCommand(IPlayerSession session, string cmdName);
        bool CanViewVar(IPlayerSession session);
        void SetGroup(IPlayerSession session, ConGroupIndex newGroup);
    }
}
