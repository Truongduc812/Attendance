﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Interfaces
{
  public interface IEntityId
  {
    [Key]
    abstract long Id { get; set; }
  }
}
