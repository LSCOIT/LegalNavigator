using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Access2Justice.Shared.Models.Integration
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
        [JsonProperty(PropertyName = "displayLabel")]
        public string DisplayLabel { get; set; }

        [JsonProperty(PropertyName = "data")]
        public string Data { get; set; }

        [JsonProperty(PropertyName = "dataType")]
        public ConditionDataType DataType { get; set; }
    }
    
    public class Expression
    {
        [JsonProperty(PropertyName = "condition")]
        public Condition Condition { get; set; }

        [JsonProperty(PropertyName = "operatorName")]
        public Operator Operator { get; set; }

        [JsonProperty(PropertyName = "variable")]
        public string Variable { get; set; }
    }


    public class AcceptanceCriteria
    {
        /// <summary>
        /// General description of the acceptance criteria by the organization
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Expressions that can be evaluated to determine eligibility for service,
        /// e.g. Age GreaterThan 65
        /// </summary>
        [JsonProperty(PropertyName = "evaluatedRequirements")]
        public IEnumerable<Expression> EvaluatedRequirements { get; set; }
    }
}