using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models
{
    public enum ConditionDataType
    {
        Text, 
        Date, 
        Number,
        TrueFalse
    }

    public enum Operator
    {
        LessThan,
        EqualTo,
        GreatherThan,
        Contains
    }

    /// <summary>
    /// Represents condition to be evaluated, 
    /// e.g. person's age
    /// </summary>
    public class Condition
    {
        public string DisplayLabel { get; set; }

        public string Data { get; set; }

        public ConditionDataType DataType { get; set; }
    }


    public class Expression
    {
        public Condition Condition { get; set; }

        public Operator Operator { get; set; }

        public string Variable { get; set; }
    }


    public class AcceptanceCriteria
    {
        /// <summary>
        /// General description of the acceptance criteria by the organization
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Expressions that can be evaluated to determine eligibility for service,
        /// e.g. Age GreaterThan 65
        /// </summary>
        public IEnumerable<Expression> EvaluatedRequirements { get; set; }
    }
}