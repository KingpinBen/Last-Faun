using UnityEngine;
using System.Collections;

public static class LayerMasks
{
    public const int IGNORE_RAY = ~( 1 << 2 );
    public const int IGNORE_RAY_GEST = ~( 1 << 2 | 1 << 9 );
    public const int IGNORE_RAY_GEST_BLOCK = ~( 1 << 2 | 1 << 8 );
    public const int IGNORE_GEST_BLOCK = ~( 1 << 8 | 1 << 9 );
    public const int HIT_BLOCK_TARGET = ( 1 << 8 );
}
