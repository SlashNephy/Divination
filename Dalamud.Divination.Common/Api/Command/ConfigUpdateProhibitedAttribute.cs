using System;

namespace Dalamud.Divination.Common.Api.Command
{
    /// <summary>
    /// このフィールドがコマンド経由で更新されないようにします。このクラスは継承できません。
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ConfigUpdateProhibitedAttribute : Attribute
    {
    }
}
