// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* ProcessorFeatures.cs -- возможности процессора
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32;

/// <summary>
/// Возможности процессора.
/// </summary>
public enum ProcessorFeatures
{
    /// <summary>
    /// Pentium: в редких случаях может возникнуть ошибка точности
    /// с плавающей запятой.
    /// </summary>
    PF_FLOATING_POINT_PRECISION_ERRATA,

    /// <summary>
    /// <para>Операции с плавающей запятой эмулируются с помощью
    /// программного эмулятора.</para>
    /// <para>Эта функция возвращает ненулевое значение, если
    /// эмулируются операции с плавающей запятой; в противном случае
    /// возвращается ноль.</para>
    /// <para>Windows NT 4.0: эта функция возвращает ноль, если
    /// эмулируются операции с плавающей запятой; в противном случае
    /// возвращается ненулевое значение. Такое поведение является
    /// ошибкой, исправленной в более поздних версиях.</para>
    /// </summary>
    PF_FLOATING_POINT_EMULATED,

    /// <summary>
    /// Доступна двойная операция сравнения и обмена (Pentium,
    /// MIPS и Alpha).
    /// </summary>
    PF_COMPARE_EXCHANGE_DOUBLE,

    /// <summary>
    /// Доступен набор инструкций MMX.
    /// </summary>
    PF_MMX_INSTRUCTIONS_AVAILABLE,

    /// <summary>
    /// ???
    /// </summary>
    PF_PPC_MOVEMEM_64BIT_OK,

    /// <summary>
    /// ???
    /// </summary>
    PF_ALPHA_BYTE_INSTRUCTIONS,

    /// <summary>
    /// Доступен набор инструкций SSE.
    /// </summary>
    PF_XMMI_INSTRUCTIONS_AVAILABLE,

    /// <summary>
    /// Доступен набор инструкций 3D-Now.
    /// </summary>
    PF_3DNOW_INSTRUCTIONS_AVAILABLE,

    /// <summary>
    /// Доступна инструкция RDTSC.
    /// </summary>
    PF_RDTSC_INSTRUCTION_AVAILABLE,

    /// <summary>
    /// Процессор поддерживает PAE.
    /// </summary>
    PF_PAE_ENABLED,

    /// <summary>
    /// Доступен набор инструкций SSE2.
    /// </summary>
    PF_XMMI64_INSTRUCTIONS_AVAILABLE
}
