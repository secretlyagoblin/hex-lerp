﻿using Hex.Geometry.Blerpables;
using Hex.Geometry.Interfaces;
using Hex.Geometry.Vectors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Hex.Geometry
{

    public class HexGroup<T> : IHexGroup<T> where T : IHexData<T>
    {
        private IHexSetQueryable<T> _hexes;

        public IEnumerable<IHex<T>> GetHexes() => _hexes;

        public HexGroup(IHexSetQueryable<T> blerps)
        {
            _hexes = blerps;
        }

        /// <summary>
        /// Creates a new hexgroup given a parent.
        /// </summary>
        /// <param name="parent"></param>
        private HexGroup(HexGroup<T> parent, int rosetteSize)
        {
            var hoods = parent.GetNeighbourhoods();

            var hexes = new List<IHex<T>>();

            for (int i = 0; i < hoods.Length; i++)
            {         
                var hood = hoods[i];

                var cells = hood.Subdivide(rosetteSize);

                for (int u = 0; u < cells.Length; u++)
                {
                    hexes.Add(cells[u]);
                }
            }

            _hexes = parent._hexes.Duplicate(hexes);
        }

        /// <summary>
        /// Get an array of neighbourhoods that allow all cells to be subdivided
        /// </summary>
        /// <returns></returns>
        private Neighbourhood<T>[] GetNeighbourhoods()
        {
            var hood = new Neighbourhood<T>[_hexes.Count()];

            var count = 0;

            foreach (var hexDictEntry in _hexes)
            {
                var hexes = new Hex<T>[6];

                var neighbours = Neighbourhood<T>.Neighbours;

                for (int i = 0; i < 6; i++)
                {
                    var key = hexDictEntry + neighbours[i];

                    hexes[i] = new Hex<T>(key,_hexes[key].Payload);
                    
                }

                hood[count] = new Neighbourhood<T>(                
                    new Hex<T>(hexDictEntry,hexDictEntry.Payload),
                    hexes[0],
                    hexes[1],
                    hexes[2],
                    hexes[3],
                    hexes[4],
                    hexes[5])
                ;
                count++;
            }

            return hood;
        }

        /// <summary>
        /// Subdivide this hexgroup.
        /// </summary>
        /// <returns></returns>
        public HexGroup<T> SubdivideThree()
        {
            //Debug.Log("Starting subdivide");
            return new HexGroup<T>(this,3);
        }

        public IHexGroup<T> Subdivide(int amount)
        {
            //Debug.Log("Starting subdivide");
            return new HexGroup<T>(this, amount);
        }

        /// <summary>
        /// Subdivide this hexgroup returning results for debugging purposes.
        /// </summary>
        /// <returns></returns>
        //public HexGroup DebugSubdivide()
        //{
        //    return new HexGroup(this, true);
        //}

        /*

        /// <summary>
        /// Hypothetical function that subdivides and returns a specific subset, based on a function, like Where() in linq.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public HexGroup GetSubGroup(Func<Hex, bool> func)
        {
            var inside = _inside.Where(x => func(x.Value)).ToDictionary(x => x.Key, x => x.Value);


            return new HexGroup(inside, GetBorderOfSubgroup(inside));
        }

        public List<HexGroup> GetSubGroups(Func<Hex, int> func)
        {
            return _inside.GroupBy(x => func(x.Value))
                .Select(x =>
                {
                    var inside = x.ToDictionary(y => y.Key, y => y.Value);
                    return new HexGroup(inside, GetBorderOfSubgroup(inside));
                }).ToList();
        }

        private Dictionary<Vector3Int, Hex> GetBorderOfSubgroup(Dictionary<Vector3Int,Hex> subgroup)
        {
            var border = new Dictionary<Vector3Int, Hex>();

            var subgroupInternalEdgeHexes = new List<Vector2Int>();

            foreach (var hexDictEntry in subgroup)
            {
                var neighbours = Neighbourhood.Neighbours;

                for (int i = 0; i < 6; i++)
                {
                    var key = hexDictEntry.Key + neighbours[i];

                    if (border.ContainsKey(key))
                    {
                        continue;
                    }
                    if (subgroup.ContainsKey(key))
                    {
                        continue;
                    }
                    if (_inside.ContainsKey(key))
                    {
                        border.Add(key, new Hex(_inside[key], true));
                    }
                    else if (_border.ContainsKey(key))
                    {

                        border.Add(key, new Hex(_border[key], true));
                    }
                    else
                    {
                        Debug.LogError("The subregion being generated has invalid neighbours.");
                    }
                }


            }

            return border;
        }

        #endregion

        #region ForEach

        public HexGroup ForEach(Func<Hex, HexPayload> func)
        {
            foreach (var pair in _inside.ToList())
            {
                var hex = pair.Value;
                hex.Payload = func(hex);
                _inside[pair.Key] = hex;
            }

            return this;
        }

        public HexGroup ForEach(Func<Hex, int, HexPayload> func)
        {
            var i = 0;

            foreach (var pair in _inside.ToList())
            {
                var hex = pair.Value;
                hex.Payload = func(hex, i);
                _inside[pair.Key] = hex;
                i++;
            }

            return this;
        }

        #endregion

        #region ToMesh + ToGameObjects

        private static Mesh ToMesh(Dictionary<Vector3Int, Hex> dict, Func<Hex,float> heightCalculator)
        {
            var verts = new Vector3[dict.Count * 3 * 6];
            var tris = new int[dict.Count * 6 * 3];
            var colors = new Color[dict.Count * 3 * 6];

            var count = 0;

            foreach (var item in dict)
            {
                var index2d = item.Value.Index.Index2d;
                var center = item.Value.Index.Position3d;
                var isOdd = item.Key.y % 2 != 0;

                var yHeight = Vector3.up * heightCalculator(item.Value);

                for (int i = 0; i < 6; i++)
                {
                    var index = (count * 3 * 6) + (i * 3);
                    verts[index] = center + yHeight;
                    verts[index + 1] = GetPointyCornerXZ(item.Value, i)+ yHeight;
                    verts[index + 2] = GetPointyCornerXZ(item.Value, i+1) + yHeight;

                    if (isOdd)
                    {
                        verts[index].x += 0.5f;
                        verts[index + 1].x += 0.5f;
                        verts[index + 2].x += 0.5f;
                    }

                    verts[index] = verts[index].AddNoiseOffset(1);
                    verts[index + 1] = verts[index + 1].AddNoiseOffset(1);
                    verts[index + 2] = verts[index + 2].AddNoiseOffset(1);

                    tris[index] = index;
                    tris[index + 1] = index + 1;
                    tris[index + 2] = index + 2;

                    colors[index]         = item.Value.Payload.Color;
                        colors[index+1]   = item.Value.Payload.Color;
                    colors[index + 2] = item.Value.Payload.Color;
                }
                count++;
            }

            return new Mesh()
            {
                vertices = verts,
                triangles = tris,
                colors = colors
            };

            Vector3 GetPointyCornerXZ(Hex hex, int i)
            {
                var index2d = hex.Index.Index2d;
                var angle_deg = 60f * i - 30f;
                var angle_rad = Mathf.PI / 180f * angle_deg;
                return new Vector3(index2d.x + Hex.HalfHex * Mathf.Cos(-angle_rad), 0,
                             (index2d.y * Hex.ScaleY) + Hex.HalfHex * Mathf.Sin(-angle_rad));
            }
        }

        private static Mesh ToMesh(Dictionary<Vector3Int, Hex> dict)
        {
            return ToMesh(dict, x => 0);
        }

        private static void ToGameObjects(Dictionary<Vector3Int, Hex> dict, GameObject prefab)
        {
            var count = 0;

            foreach (var item in dict)
            {
                var position3d = item.Value.Index.Position3d;

                var obj = GameObject.Instantiate(prefab);

                obj.name = item.Key.ToString();
                obj.transform.position = position3d;
                var payload = obj.AddComponent<PayloadData>();
                item.Value.Payload.PopulatePayloadObject(payload);
                payload.IsBorder = item.Value.IsBorder;
                payload.NeighbourhoodData= item.Value.DebugData;

                if (Hex.IsInvalid(item.Value))
                {
                    payload.name += $"_NULL";
                }       

                count++;
            }
        }

        public Mesh ToMesh()
        {
            return HexGroup.ToMesh(_inside);
        }

        public Mesh ToMesh(Func<Hex, float> heightCalculator)
        {
            return HexGroup.ToMesh(_inside,heightCalculator);
        }

        public void ToGameObjects(GameObject prefab)
        {
            HexGroup.ToGameObjects(_inside, prefab);
        }

        public void ToGameObjectsBorder(GameObject prefab)
        {
            HexGroup.ToGameObjects(_border, prefab);
        }

        public Mesh ToMeshBorder()
        {
            return HexGroup.ToMesh(_border);
        }

        private (Vector3[] vertices, int[] triangles)  ToNetwork(Func<HexPayload,float> zOffset)
        {
            var count = 0;
            var indexes = _inside.Values.Select(x => x.Index);
            var verts = indexes.ToDictionary(x => x.Index3d, x => { var i = count; count++; return i; });
            HashSet<Vector3Int> triangles = new HashSet<Vector3Int>();

            foreach (var index in indexes)
            {
                var neighbourhood = Neighbourhood.Neighbours;

                for (int i = 0; i < neighbourhood.Length; i++)
                {
                    var index3d = index.Index3d;
                    var n1 = index3d + neighbourhood[i];
                    var n2 = index3d + (i < neighbourhood.Length - 1 ? neighbourhood[i + 1]: neighbourhood[0]);

                    if(!(verts.ContainsKey(n1) && verts.ContainsKey(n2)))
                        continue;

                    var index2d = index.Index2d;
                    var n12d = HexIndex.Get2dIndex(n1);
                    var n22d = HexIndex.Get2dIndex(n2);

                    //Determine triangle shape

                    var threePoints = new Vector3Int[3];
                    var indexIsOdd = index2d.y % 2 != 0;
                    var n1IsOdd = n12d.y % 2 != 0;
                    var n2IsOdd = n22d.y % 2 != 0;

                    var testIndex = indexIsOdd? index2d.x+0.5f: index2d.x;
                    var testn1 = n1IsOdd ? n12d.x + 0.5f : n12d.x;
                    var testn2 = n2IsOdd ? n22d.x + 0.5f : n22d.x;

                    if (testIndex<testn1 && testIndex< testn2)
                    {
                        threePoints[0] = HexIndex.Get3dIndex(index2d);
                        threePoints[1] = HexIndex.Get3dIndex(n12d);
                        threePoints[2] = HexIndex.Get3dIndex(n22d);
                    }
                    else if (testn1< testIndex && testn1< testn2)
                    {
                        threePoints[0] = HexIndex.Get3dIndex(n12d);
                        threePoints[1] = HexIndex.Get3dIndex(index2d);
                        threePoints[2] = HexIndex.Get3dIndex(n22d);
                    }
                    else if (testn2< testIndex && testn2< testn1)
                    {
                        threePoints[0] = HexIndex.Get3dIndex(n22d);
                        threePoints[1] = HexIndex.Get3dIndex(n12d);
                        threePoints[2] = HexIndex.Get3dIndex(index2d);
                    }
                    else
                    {
                        Debug.Log("Whelp");
                    }

                    if (threePoints[1].y < threePoints[2].y)
                    {
                        var temp = threePoints[1];
                        threePoints[1] = threePoints[2];
                        threePoints[2] = temp;
                    }

                    var tri = new Vector3Int(verts[threePoints[0]], verts[threePoints[1]], verts[threePoints[2]]);

                    if (triangles.Contains(tri))
                        continue;
                    triangles.Add(tri);
                }
            }

            var vertices = indexes.Select(x => x.Position3d).ToArray();
            var finalTriangles = triangles.SelectMany(x => new[] { x.x, x.y, x.z }).ToArray();

            return (vertices, finalTriangles);
            //,indexes.Select(x => new Vector3(x.x, 0, x.y)).ToArray()
            //triangles.SelectMany(x => new[]{ x.x, x.y, x.z }).ToArray(),
            // _inside.Values.Select(x => x.Payload).ToArray());
        }

        public Mesh ToConnectedMesh(Func<HexPayload, float> heightFunc, Func<HexPayload, Color> colorFunc)
        {
            (var vertices, var triangles) = this.ToNetwork(heightFunc);

            var mesh = new Mesh()
            {
                vertices = vertices,
                triangles = triangles,
                colors = _inside.Select(x => colorFunc(x.Value.Payload)).ToArray()
            };

            mesh.RecalculateNormals();

            return mesh;
        }

        #endregion

        #region graphs

        public T ToGraph<T>(Func<HexPayload, int> regionIndentifier, Func<HexPayload, int[]> regionConnector) where T: Graph<HexPayload>
        {
            (var vertices, var triangles) = this.ToNetwork(x => 0);

            //Have to do this as you can't generate a T with a specific constructor
            var type = Activator.CreateInstance(typeof(T),
                vertices,
                triangles,
                this._inside.Select(x => x.Value.Payload).ToArray(),
                regionIndentifier,
                regionConnector) as T;

            return type;
        }

        public HexGroup MassUpdateHexes(HexPayload[] data)
        {
            return this.ForEach((x, i) => data[i]);
        }

        #endregion

        */


    }


}