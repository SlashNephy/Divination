using System;

namespace Dalamud.Divination.Common.Api.Utilities;

/// <summary>
///     このフィールドがコマンド経由で更新されないようにします。このクラスは継承できません。
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class UpdateProhibitedAttribute : Attribute
{
}
