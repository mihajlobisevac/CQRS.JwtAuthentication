﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Common.Models
{
    public class Result
    {
        private Result(bool succeeded, IEnumerable<string> errors)
        {
            Succeeded = succeeded;
            Errors = errors.ToArray();
        }

        public bool Succeeded { get; private set; }

        public string[] Errors { get; private set; }

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }
}
