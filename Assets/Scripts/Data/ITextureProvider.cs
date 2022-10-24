using UnityEngine;

namespace Data
{
public interface ITextureProvider
{
    Texture2D Get(string id);
}
}