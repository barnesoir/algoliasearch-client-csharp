/*
* Copyright (c) 2018 Algolia
* http://www.algolia.com/
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using Algolia.Search.Exceptions;
using Algolia.Search.Models.Common;
using System;
using System.Threading.Tasks;

namespace Algolia.Search.Models.Mcm
{
    /// <summary>
    /// Waitable response of assignUserid method
    /// </summary>
    public class AssignUserIdResponse : IAlgoliaWaitableResponse
    {
        internal Func<string, UserIdResponse> GetUserId { get; set; }

        /// <summary>
        /// The user Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Date of creation of the userId
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Wait until the userID is created on the API
        /// Loop until httpErrorCode != 404
        /// </summary>
        public void Wait()
        {
            while (true)
            {
                try
                {
                    GetUserId(UserId);
                }
                catch (AlgoliaApiException ex)
                {
                    // Loop until we have found the userID
                    if (ex.HttpErrorCode == 404)
                    {
                        Task.Delay(1000);
                        continue;
                    }

                    throw;
                }

                break;
            }
        }
    }
}