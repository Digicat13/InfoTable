using BitsOrchestra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitsOrchestra.Data
{
    public class InitialData
    {
        public static void Initialize(ApplicationContext context)
        {
            if(!context.Records.Any())
            {
                context.Records.AddRange(
                    new Record
                    {
                 
                        Name = "Ivan Ivanov",
                        DateOfBirth = new DateTime(1997, 7, 12),
                        Married = true,
                        Phone = "355035555",
                        Salary = 15555
                    },
                      new Record
                      {
                  
                          Name = "John Doe",
                          DateOfBirth = new DateTime(1998, 8, 11),
                          Married = false,
                          Phone = "666666666",
                          Salary = 20555
                      }
                    );
                context.SaveChanges();
            }
        }
    }
}
