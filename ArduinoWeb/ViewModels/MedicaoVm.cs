﻿namespace ArduinoWeb.ViewModels
{
    public class MedicaoVm
    {
        public DateTime? DataMedicao { get; set; }
        public string NomeLocalizacao { get; set; }
        public bool Movimento { get; set; }

        public string GoogleDate => (DataMedicao.HasValue)
  ? string.Format("Date({0})", DataMedicao.Value.ToString("yyyy,M,d,H,m,s,f"))
  : string.Empty;
        public string DateOnlyString => DataMedicao?.ToString("yyyy-MM-dd") ?? string.Empty;
        public string TimeOnlyString => DataMedicao?.ToString("hh:mm:ss tt") ?? string.Empty;


    }
}
