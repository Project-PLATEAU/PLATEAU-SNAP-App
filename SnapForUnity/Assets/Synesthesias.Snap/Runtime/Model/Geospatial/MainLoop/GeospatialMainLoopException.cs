using System;

namespace Synesthesias.Snap.Runtime
{
    /// <summary>
    /// GeospatialのメインループのException
    /// </summary>
    public class GeospatialMainLoopException : Exception
    {
        /// <summary>
        /// メインループの状態
        /// </summary>
        public readonly GeospatialMainLoopStateType StateType;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GeospatialMainLoopException(GeospatialMainLoopStateType stateType)
        {
            StateType = stateType;
        }
    }
}