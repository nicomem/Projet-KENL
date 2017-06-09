var mipmap : float = -0.5;
function Start () {
	for(var i = 0; i < GetComponent.<Renderer>().sharedMaterials.Length; i++){
		GetComponent.<Renderer>().sharedMaterials[i].mainTexture.mipMapBias = mipmap;
		var bumpMapTexture : Texture = GetComponent.<Renderer>().sharedMaterials[i].GetTexture("_BumpMap");
		bumpMapTexture.mipMapBias = mipmap;
		GetComponent.<Renderer>().sharedMaterials[i].SetTexture("_BumpMap", bumpMapTexture);
	}
	//renderer.material.mainTexture.mipMapBias = mipmap;
}