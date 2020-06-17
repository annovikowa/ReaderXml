using ReaderXml.Fillers;
using ReaderXml.Models;
using System.Xml;
using System;

namespace ReaderXml.ECPT
{
    /// <summary>
    /// Кадастровый план территории.
    /// </summary>
    public class ECPTFiller : IFiller<CadastralPlanTerritory>
    {
        public void Fill(CadastralPlanTerritory model, XmlReader reader)
        {
            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "cadastral_block":
                                {
                                    var inner = reader.ReadSubtree();
                                    var cadBlock = new CadastralBlock();
                                    var filler = new CadastralBlockFiller();
                                    filler.Fill(cadBlock, inner);
                                    model.CadastralBlocks.Add(cadBlock);
                                    inner.Close();
                                }
                                break;
                            case "organ_registr_rights":
                                {
                                    model.OrganRegistrRights = reader.ReadElementContentAsString();
                                }
                                break;
                            case "date_formation":
                                {
                                    model.DateFormation = reader.ReadElementContentAsDateTime();
                                }
                                break;
                            case "registration_number":
                                {
                                    model.RegistrationNumber = reader.ReadElementContentAsString();
                                }
                                break;
                            case "initials_surname":
                                {
                                    model.Official += $"{reader.ReadElementContentAsString()}, ";
                                }
                                break;
                            case "full_name_position":
                                {
                                    model.Official += $"{reader.ReadElementContentAsString()} ";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                //log
            }
            

        }
    }
}
