﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algoritmo.Core.Domain.Model
{
    public class TrabajoObjetivo
    {
        public int Id { get; set; }
        public int Objetivo { get; set; }
        public bool IsOverdue { get; set; }
    }
}
