using ConverterXlsx.DB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ConverterXlsx.DB
{
    /// <summary>
    /// Основной класс для доступа к БД. Все обращения должны идти через него.
    /// </summary>
    public class ConverterXlsxRepository : IDisposable
    {
        private ConverterXlsxContext _converterXlsxContext;
        public ConverterXlsxContext Context
        {
            get
            {
                
                if (_converterXlsxContext == null || _converterXlsxContext.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    _converterXlsxContext?.Dispose();
                    _converterXlsxContext = new ConverterXlsxContext();
                    int attemp = 0;
                    while (_converterXlsxContext.Database.Connection.State != System.Data.ConnectionState.Open && attemp < 2) //попытки подключения не обязательны, просто взял на основе уже используемого кода
                    {
                        attemp++;
                        if (attemp > 0)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                        _converterXlsxContext.Database.Connection.Open();
                    }
                }
                return _converterXlsxContext;
            }
        }

        public static ConverterXlsxRepository GetInstance() => new ConverterXlsxRepository();

        public int SaveChanges()
        {
            return Context?.SaveChanges() ?? 0;
        }

        public Task<int> SaveChangesAsync()
        {
            return Context?.SaveChangesAsync() ?? Task.FromResult(0);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }

        public ICollection<Conversion> GetConversions()
        {
            //важно при получении коллекций из БД делать ToArray\ToList и т.д. т.к. в случае обработки их циклом будут возникать исключения
            return Context.Conversions.ToArray();
        }

        public Conversion GetConversion(string id) => Context.Conversions.Find(id);

        public void SaveError(string errorDescription, Conversion conversion)
        {
            Error error = new Error { Conversion = conversion, ErrorDescription = errorDescription };
            Context.Errors.Add(error);
        }
    }
}