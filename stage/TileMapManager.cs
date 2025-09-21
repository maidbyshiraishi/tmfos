using Godot;
using Godot.Collections;
using tmfos.system;

namespace tmfos.stage;

/// <summary>
/// タイルマップ制御
/// </summary>
public partial class TileMapManager : Node2D
{
    [Export]
    public float ObservationHoleRadius { get; set; } = 80f;

    [Export]
    public int ObservationHoleVertexCount { get; set; } = 12;

    public static readonly string BackgroundLayerPath = "Background";
    public static readonly string GroundLayerPath = "Ground";
    public static readonly string ForegroundLayerPath = "Foreground";
    public static readonly string VeilLayerPath = "ObservationHole/Veil";

    private Dictionary<string, TileMapLayer> _layers = [];
    private bool _transparent = false;
    private Polygon2D _observationHole;

    private Dictionary<int, Vector2I> _crackTiles = new() {
        {5, new(1,5)},
        {4, new(2,5)},
        {3, new(3,5)},
        {2, new(4,5)},
        {1, new(5,5)}
    };

    public override void _Ready()
    {
        _layers.Add(BackgroundLayerPath, GetNode<TileMapLayer>(BackgroundLayerPath));
        _layers.Add(GroundLayerPath, GetNode<TileMapLayer>(GroundLayerPath));
        _layers.Add(ForegroundLayerPath, GetNode<TileMapLayer>(ForegroundLayerPath));
        _layers.Add(VeilLayerPath, GetNode<TileMapLayer>(VeilLayerPath));
        MakeObservationHole();
    }

    private void MakeObservationHole()
    {
        _observationHole = GetNode<Polygon2D>("ObservationHole");
        Vector2[] list = new Vector2[ObservationHoleVertexCount];
        Vector2 line = new(ObservationHoleRadius, 0f);
        int length = list.Length;
        float angle = Mathf.DegToRad(360f / length);
        list[0] = line;

        for (int i = 1; i < length; i++)
        {
            line = line.Rotated(angle);
            list[i] = line;
        }

        _observationHole.Polygon = list;
    }

    /// <summary>
    /// タイルへの接触を確認し、壊れる床の制御を行う
    /// </summary>
    /// <param name="coords">座標</param>
    /// <param name="playSe">効果音を鳴らすか</param>
    public bool BlockCollided(Vector2I coords, bool playSe)
    {
        int crackCount = GetCustomDataAsInt(ForegroundLayerPath, coords, "CrackCount");

        if (crackCount == 0)
        {
            return false;
        }

        if (playSe)
        {
            GetNode<SePlayer>("/root/SePlayer").Play("crack");
        }

        TileMapLayer foreground = _layers[ForegroundLayerPath];
        TileMapLayer ground = _layers[GroundLayerPath];
        Vector2I fcoords = foreground.LocalToMap(coords);

        if (crackCount - 1 == 0)
        {
            ground.EraseCell(ground.LocalToMap(coords));
            foreground.EraseCell(fcoords);
            return true;
        }

        foreground.SetCell(fcoords, 0, _crackTiles[crackCount - 1]);
        return true;
    }

    /// <summary>
    /// タイルデータを取得する
    /// </summary>
    /// <param name="layer">レイヤー</param>
    /// <param name="coords">座標</param>
    /// <returns>TileData</returns>
    public TileData GetTileData(string layerPath, Vector2I coords)
    {
        TileMapLayer layer = _layers[layerPath];
        Vector2I local = layer.LocalToMap(coords);
        TileData tiledataHead = layer.GetCellTileData(local);
        return tiledataHead;
    }

    /// <summary>
    /// 対象座標のタイルに設定されたカスタムデータを
    /// bool値として取得する
    /// </summary>
    /// <param name="layer">レイヤー</param>
    /// <param name="coords">座標</param>
    /// <param name="name">カスタムデータ名</param>
    /// <returns>カスタムデータ</returns>
    public bool GetCustomDataAsBool(string layerPath, Vector2I coords, string name)
    {
        Variant v = GetCustomDataAsVariant(layerPath, coords, name);
        return v.VariantType is Variant.Type.Bool && v.AsBool();
    }

    /// <summary>
    /// 対象座標のタイルに設定されたカスタムデータを
    /// int値として取得する
    /// </summary>
    /// <param name="layer">レイヤー</param>
    /// <param name="coords">座標</param>
    /// <param name="name">カスタムデータ名</param>
    /// <returns>カスタムデータ</returns>
    public int GetCustomDataAsInt(string layerPath, Vector2I coords, string name)
    {
        Variant v = GetCustomDataAsVariant(layerPath, coords, name);
        return v.VariantType is Variant.Type.Int ? v.AsInt32() : 0;
    }

    /// <summary>
    /// 対象座標のタイルに設定されたカスタムデータを
    /// Variant値として取得する
    /// </summary>
    /// <param name="layer">レイヤー</param>
    /// <param name="coords">座標</param>
    /// <param name="name">カスタムデータ名</param>
    /// <returns>カスタムデータ</returns>
    public Variant GetCustomDataAsVariant(string layerPath, Vector2I coords, string name)
    {
        TileMapLayer layer = _layers[layerPath];
        Vector2I local = layer.LocalToMap(coords);
        TileData tileData = layer.GetCellTileData(local);
        return tileData is null ? new() : tileData.GetCustomData(name);
    }

    internal void OpenObservationHole(bool flag, Vector2 position)
    {
        // todo: 切り抜かないとき、はるか彼方の画面外を切り抜いているが、一時的に無効にできないのか？
        _observationHole.Offset = flag ? position : new(-2000, -2000);
    }
}
