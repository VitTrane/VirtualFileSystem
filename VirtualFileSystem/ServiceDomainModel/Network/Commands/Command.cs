using System;

namespace ServiceDomainModel.Commands
{
    [Serializable]
    public abstract class Command
    {
        public TypeCommand ComandType { get; private set; }

        public Command(TypeCommand typecomand)
        {
            ComandType = typecomand;
        }
    }
}
