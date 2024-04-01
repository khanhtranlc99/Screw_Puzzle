using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//===============================================================
//Developer:  CuongCT
//Company:    Rocket Studio
//Date:       2019
//================================================================
public class GException : Exception {
    public GErrorCode ErrorCode
    {
        get { return Error.Error; }
    }

    public GError Error { get; private set; }

    public GException(GError error) : base(error.ErrorMessage)
    {
        Error = error;
       
    }
}
