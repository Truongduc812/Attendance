using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Interfaces
{
  public  interface IEntityName
  {
    abstract string Name { get; set; }
  }
}
