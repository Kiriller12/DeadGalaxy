/*
=========================================================

DeadGalaxy project

All right reserved
Kabanov Kirill (Kiriller12) © 2024

Licensed under GNU General Public License v3.0
Full license text available in LICENCE file

=========================================================
*/

namespace DeadGalaxy.Core.Resources
{
    /// <summary>
    /// Game entity resource type
    /// </summary>
    internal enum EntityResourceType
    {
        Unknown = 0,
        Static,
        Dynamic,
        World,
        Camera,
        PointLight
    }
}
