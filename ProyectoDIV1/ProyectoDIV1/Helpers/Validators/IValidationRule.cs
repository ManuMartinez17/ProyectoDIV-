﻿using System;
namespace ProyectoDIV1.Validators
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }
        bool Check(T value);
    }
}
