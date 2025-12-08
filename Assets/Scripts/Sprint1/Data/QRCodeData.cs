using System;
using UnityEngine;

namespace SmartRetailAR.Data
{
    [Serializable]
    public class QRCodeData
    {
        public string code;
        public string productId;
        public DateTime scanTime;
        public string scanLocation;

        public QRCodeData(string code, string productId)
        {
            this.code = code;
            this.productId = productId;
            this.scanTime = DateTime.Now;
            this.scanLocation = "Unknown";
        }
    }
}
