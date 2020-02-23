﻿// <auto-generated />
//-----------------------------------------------------------------------------
// <copyright file="IacRecord.cs" company="WheelMUD Development Team">
//   Copyright (c) WheelMUD Development Team. See LICENSE.txt. This file is
//   subject to the Microsoft Public License. All other rights reserved.
// </copyright>
// <summary>
//   auto-generated by EntityRecord.cst on 7/7/2012 4:24:09 PM
// </summary>
//-----------------------------------------------------------------------------

namespace WheelMUD.Data.Entities
{
    using ServiceStack.DataAnnotations;

    /// <summary>Represents a single IAC row in the IAC table.</summary>
    [Alias("IAC")]
    public partial class IacRecord
    {
        [AutoIncrement]
        public virtual long ID { get; set; }
        public virtual string Name { get; set; }
        public virtual int OptionCode { get; set; }
        public virtual bool NegotiateAtConnect { get; set; }
        public virtual bool RequiresSubNegotiation { get; set; }
        public virtual string SubNegAssembly { get; set; }
        public virtual string NegotiationStartValue { get; set; }
    }
}