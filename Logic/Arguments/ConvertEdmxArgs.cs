﻿using EdmxConverter.DomainLogic.Types;

namespace EdmxConverter.DomainLogic.Converting.Arguments
{
    public abstract class ConvertEdmxArgs
    {
        protected ConvertEdmxArgs(Edmx model, EdmxTypeEnum source, EdmxTypeEnum target)
        {
            Source = source;
            Target = target;
            Model = model;
        }

        public EdmxTypeEnum Source { get; }
        public EdmxTypeEnum Target { get; }
        private Edmx Model { get; }
    }
}