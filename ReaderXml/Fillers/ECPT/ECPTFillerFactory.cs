using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReaderXml.ECPT;
using ReaderXml.Models;

namespace ReaderXml.Fillers.ECPT
{
    public static class ECPTFillerFactory
    {
        public static IFiller<T> GetFiller<T>(T model) where T : CadastralObject
        {            
            switch (model)
            {
                case Parcel _:
                    {
                        return (IFiller<T>)new LandFiller();
                    }
                case Building _:
                    {
                        return (IFiller<T>)new BuildFiller();
                    }
                case Construction _:
                    {
                        return (IFiller<T>)new ConstructionFiller();
                    }
                case Uncompleted _:
                    {
                        return (IFiller<T>)new ObjectUnderConstructionFiller();
                    }
                case Bound _:
                    {
                        return (IFiller<T>)new SubjectBoundaryFiller();
                    }
                case Zone _:
                    {
                        return (IFiller<T>)new ZoneFiller();
                    }
                case OMSPoint _:
                    {
                        return (IFiller<T>)new OMSPointFiller();
                    }
                case SurveyingProject _:
                    {
                        return (IFiller<T>)new SurveyingProjectFiller();
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
