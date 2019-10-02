﻿using System.Collections.Generic;
using System.Reflection;

namespace Tracer
{
    public class ThreadInformation
    {
        public ThreadInformation(int id, List<MethodInformation> threadMethods)
        {
            Methods = new List<MethodInformation>();
            Methods = threadMethods;
            Id = id;
        }
        
        public int Id { get; }

        private double _executionTime;

        public double ExecutionTime
        {
            get
            {
                if (Methods.Count > 0)
                {
                    _executionTime = Methods[0].ExecutionTime;      /////////////////////
                }
                return _executionTime;
            }
            private set => _executionTime = value;
        }

        public List<MethodInformation> Methods { get; }
    }
}