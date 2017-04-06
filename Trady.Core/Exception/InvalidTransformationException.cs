using System;

namespace Trady.Core.Exception
{
    public class InvalidTransformationException : System.Exception
    {
        private Type _sourceType, _targetType;

        public InvalidTransformationException(Type sourceType, Type targetType)
        {
            _sourceType = sourceType;
            _targetType = targetType;
        }

        public override string Message => $"Invalid transformation from {_sourceType.Name} to {_targetType.Name}";
    }
}