using System;
using System.Collections.Generic;
using Maddalena.ML.Model;

namespace Maddalena.ML.Geocoding
{
    public class ResultItem
    {
        private Address input;

        private IEnumerable<Address> output;

        public ResultItem(Address request, IEnumerable<Address> response)
        {
            Request = request;
            Response = response;
        }

        /// <summary>
        ///     Original input for this response
        /// </summary>
        public Address Request
        {
            get => input;
            set { input = value ?? throw new ArgumentNullException("Input"); }
        }

        /// <summary>
        ///     Output for the given input
        /// </summary>
        public IEnumerable<Address> Response
        {
            get => output;
            set { output = value ?? throw new ArgumentNullException("Response"); }
        }
    }
}