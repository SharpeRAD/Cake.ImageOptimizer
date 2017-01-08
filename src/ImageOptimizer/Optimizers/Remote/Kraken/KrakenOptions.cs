#region Using Statements
    using System;
#endregion



namespace Cake.ImageOptimizer
{
    [Serializable]
    internal class KrakenOptions
	{
        #region Constructor (2)
            public KrakenOptions()
                : this(null, false, false)
            {

            }

            public KrakenOptions(KrakenAuth pAuth, bool pWait, bool pLossy)
            {
                auth = pAuth;
                wait = pWait;
                lossy = pLossy;
            }
        #endregion





        #region Properties (3)
            public KrakenAuth auth { get; set; }

            public bool wait { get; set; }

            public bool lossy { get; set; }
        #endregion
    }
}