using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.CommonResponses
{
    public class Result
    {

        private List<Error> _errors = [];

        // is success
        public bool IsSuccess => _errors.Count == 0; // true

        // is failure
        public bool IsFailure => !IsSuccess; // false

        // errors
        public IReadOnlyList<Error> Errors => _errors;

        // apply factory pattern to control create result object

        // success result
        protected Result() { 
        

        }

        // failuer with one error 
        protected Result(Error error) {
            _errors.Add(error);
        }

        // failure with multiple errors
        protected Result(List<Error> errors) {
            _errors.AddRange(errors);
        }

        // factory method for success result
        public static Result Ok() => new();

        // factory method for failure result with one error
        public static Result Fail(Error error) => new (error);

        // factory method for failure result with multiple errors
        public static Result Fail(List<Error> errors) => new (errors);
    }




    // generic result class to hold data for functions that return data in case of success
    public class Result<TValue> : Result { 
    
        private readonly TValue _value;

        public TValue Value => _value;

        // success result with value
        private Result(TValue value) : base() {
            _value = value;
        }

        // failure result with one error
        private Result(Error error) : base(error) {
            _value = default!;
        }

        // failure result with multiple errors
        private Result(List<Error> errors) : base(errors) {
            _value = default!;
        }

        // factory method for success result with value
        public static Result<TValue> Ok(TValue value) => new (value);

        // factory method for failure result with one error
        public static new Result<TValue> Fail(Error error) => new (error);

        // factory method for failure result with multiple errors
        public static new Result<TValue> Fail(List<Error> errors) => new (errors);





        // implicit conversion operator to allow returning value directly from functions that return Result<TValue>

        public static implicit operator Result<TValue>(TValue value) => Ok(value);

        public static implicit operator Result<TValue>(Error error) => Fail(error);

        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);
    }
}
