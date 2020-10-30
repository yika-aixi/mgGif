//Create: Icarus
//ヾ(•ω•`)o
//2020-10-30 07:08
//MG.GIF

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MG.GIF
{
    public class GIFAnimation
    {
        private List<Texture> _textures;
        private List<float>   _frameTimes;

        public event Action<Texture> OnUpdateCallback; 
        
        public GIFAnimation()
        {
            _textures = new List<Texture>();
            
            _frameTimes = new List<float>();
        }

        public void Load(string path)
        {
            Load(File.ReadAllBytes(path));
        }
        
        public void Load(byte[] bs)
        {
            using( var decoder = new Decoder(bs))
            {
                while (true)
                {
                    var img = decoder.NextImage();

                    if (img != null)
                    {
                        _textures.Add(img.CreateTexture());
                        _frameTimes.Add(img.Delay / 1000f);
                    }
                    else
                        break;
                            
                }
            }    
        }

        private float _cTime;
        private int   _cFrame;

        public void Update(float time)
        {
            _cTime += time;

            if( _cTime >= _frameTimes[ _cFrame ])
            {
                _cFrame = ( _cFrame + 1 ) % _frameTimes.Count;
                _cTime   = 0.0f;
            }

            OnUpdateCallback?.Invoke(_textures[_cFrame]);
        }

        public void Update()
        {
            Update(Time.deltaTime);
        }
    }
}