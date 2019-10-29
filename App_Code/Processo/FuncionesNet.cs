using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace Componente.Procesos
{
    public class FuncionesNet
    {
        public FuncionesNet()
        {
            //
            // TODO: Agregar aquí la lógica del constructor
            //
        }
        public string CalculoTiempoTrascu(DateTime FechaInicio, DateTime FechaFin)
        {

            string formato = FechaInicio.ToString("dd/MM/yyyy hh:mm  tt");

            TimeSpan FechaTotal = FechaFin.Subtract(FechaInicio);
           
            
            string TiempoTotal = string.Empty;

            int DiasTrans = FechaTotal.Days;
            int MinutosTotales = 0;

            int DiaInicio = FechaInicio.Day;
            int MesInicio = FechaInicio.Month;
            int HoraInicio = FechaInicio.Hour;
            int MinInicio = FechaInicio.Minute;
            int AñoInicio = FechaInicio.Year;
            int DiaMaxMesInicio = DateTime.DaysInMonth(AñoInicio, MesInicio);

            int DiaFin = FechaFin.Day;
            int MesFin = FechaFin.Month;
            int HoraFin = FechaFin.Hour;
            int MinFin = FechaFin.Minute;
            int AñoFin = FechaInicio.Year;
            int DiaMaxMesFin = DateTime.DaysInMonth(AñoFin, MesFin);

            int HorasTranscurri = 0;
            int MinutosTranscurri = 0;
            int DiasExtra = 0;

            int HorasExt = 0;
            int MinsExt = 0;
            String DiaLetra = String.Format("{0:dddd}", FechaInicio);

            if (MinInicio > 0)
            {
                MinutosTotales = 60 - MinInicio;
                MinutosTotales += MinFin;
                HorasExt = (MinutosTotales / 60);
                MinsExt = MinutosTotales - 60 * HorasExt;
            }


            if (MesInicio == MesFin && DiaInicio == DiaFin)
            {
                if ((DiaLetra != "sábado") && (DiaLetra != "domingo"))
                {
                    if (HoraInicio >= 8 && HoraFin <= 19)
                    {
                        TiempoTotal = FechaTotal.ToString();
                    }
                }
            }
            else if (MesInicio == MesFin && DiaInicio != DiaFin)
            {

                for (int i = DiaInicio; i <= DiaFin; i++)
                {
                    if (DiaInicio != i)
                    {
                        DiaLetra = String.Format("{0:dddd}", FechaInicio.AddDays(DiasExtra));
                    }

                    if ((DiaLetra != "sábado") && (DiaLetra != "domingo"))
                    {
                        if (HoraInicio >= 8 && HoraInicio <= 18 && i == DiaInicio)
                        {
                            for (int e = HoraInicio + 1; e <= 18; e++)
                            {

                                HorasTranscurri += 1;

                            }
                        }
                        else if (i != DiaFin)
                        {
                            HorasTranscurri += 10;
                        }
                        else if (i == DiaFin)
                        {
                            for (int e = 9; e <= HoraFin; e++)
                            {

                                HorasTranscurri += 1;

                            }

                        }

                    }
                    DiasExtra += 1;
                }
            }


            if (MesInicio == MesFin && DiaInicio != DiaFin)
            {
                TiempoTotal = HorasTranscurri.ToString();
            }
            else if (MesInicio == MesFin && DiaInicio != DiaFin)
            {
                TiempoTotal = HorasTranscurri.ToString();
            }




            return TiempoTotal.ToString();
        }
    }
}