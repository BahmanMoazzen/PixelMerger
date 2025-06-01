using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Timeline;

public class MergersController : MonoBehaviour
{
    const string ISDANGERTAG = "IsDanger";
    public static event UnityAction<GameObject, GameObject, GameObject> OnMerge;
    public static event UnityAction<GameObject> OnPixelClicked;
    public static event UnityAction OnContactLatencyExceed;
    public static bool _isAnnounced = false;
    const float ANGULARSPEEDRANGE = 50f;
    //[SerializeField] TextMeshPro _pixelName;
    [SerializeField] CircleCollider2D _pixelCollider;
    [SerializeField] Rigidbody2D _rigidbody;
    [SerializeField] SpriteRenderer _BGSpriteRenderer;
    [SerializeField] SpriteRenderer _FGSpriteRenderer;
    [SerializeField] PixelInfo _mergerInfo;
    [SerializeField] GameObject _lineCreator;
    [SerializeField] bool _isUnicolor = false;
    [SerializeField] Sprite _unicolorSprite;
    bool _isMerging = false;
    int _pixelID;

    Animator _anim;

    [SerializeField] LayerMask _lineLayer;
    float _contactLatency = 0;

    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
    public int _PixelID
    {
        get { return _pixelID; }
    }
    public PixelInfo _MergerInfo
    {
        get
        {
            return _mergerInfo;
        }
    }
    public Sprite _BGSprite { get; set; }

    public void _LoadUnicolor()
    {
        _isUnicolor = true;
        _rigidbody.mass = 1;
        _FGSpriteRenderer.sprite = _unicolorSprite;
        _rigidbody.gravityScale = 0;
        _pixelCollider.enabled = false;

    }
    public void _LoadPixel(PixelInfo iMerger, Vector2 iVelocity, float iAngularVelocity, bool iIsinGame)
    {
        _pixelID = Random.Range(int.MinValue, int.MaxValue);
        _mergerInfo = iMerger;
        _rigidbody.mass = A.Pixels.PixelMass(iMerger.MergerOrder);
        transform.localScale = transform.localScale * A.Pixels.PixelScale(iMerger.MergerOrder);
        _BGSpriteRenderer.sprite = iMerger.MergerDeckInfo.DeckBackGround;
        _FGSpriteRenderer.sprite = iMerger.MergerSprite;
        transform.tag = A.Pixels.PixelTag(iMerger.MergerOrder);
        _rigidbody.linearVelocity = iVelocity;
        _rigidbody.angularVelocity = iAngularVelocity;
        if (iIsinGame)
        {
            _rigidbody.gravityScale = 1;
            _pixelCollider.enabled = true;
        }
        else
        {
            _rigidbody.gravityScale = 0;
            _pixelCollider.enabled = false;
        }
    }
    public void _Drop()
    {
        Destroy(_lineCreator);
        _rigidbody.gravityScale = 1;
        _rigidbody.angularVelocity = Random.Range(-ANGULARSPEEDRANGE, ANGULARSPEEDRANGE);
        _pixelCollider.enabled = true;
    }
    public void _Endanger(bool iDanger)
    {
        _anim.SetBool(ISDANGERTAG, iDanger);

    }

    private void LateUpdate()
    {
        if (!_isUnicolor)
        {
            Collider2D C2D = Physics2D.OverlapCircle(transform.position, A.Pixels.PixelGizmoRadious(_mergerInfo.MergerOrder), _lineLayer);
            if (C2D != null)
            {
                _contactLatency += Time.deltaTime;
                if (_contactLatency > 1f)
                {
                    _Endanger(true);
                }

                if (_contactLatency > A.Pixels.MAXLATENCY)
                {
                    if (!_isAnnounced)
                    {
                        _isAnnounced = true;
                        OnContactLatencyExceed?.Invoke();

                    }
                }
            }
            else
            {
                _Endanger(false);
                _contactLatency = 0;
            }
        }
        List<Collider2D> oRes = new List<Collider2D>();
        if (_pixelCollider.Overlap(oRes) > 0 && !_isMerging)
        {
            //overpaded
            if (_isUnicolor)
            {
                var c2d = oRes[0];
                if (c2d.tag.StartsWith("M"))
                {
                    _isMerging = true;
                    SoundManager._Instance._PlaySound(GameSounds.Merge);
                    PixelInfo newPixel = A.Pixels.NextPixel(c2d.GetComponent<MergersController>()._MergerInfo);
                    GameObject newGo = null;
                    if (newPixel != null)
                    {
                        newGo =
                        Instantiate(A.Pixels.PixelSkeleton
                            , c2d.gameObject.transform.position
                            , Quaternion.identity);
                        newGo.GetComponent<MergersController>()._LoadPixel(newPixel, c2d.GetComponent<Rigidbody2D>().linearVelocity, c2d.GetComponent<Rigidbody2D>().angularVelocity, true);
                    }
                    OnMerge?.Invoke(c2d.gameObject, gameObject, newGo);
                    Destroy(c2d.gameObject);
                    Destroy(gameObject);
                }
            }
            else
            {
                foreach (Collider2D c2d in oRes)
                {
                    if (c2d.CompareTag(transform.tag))
                    {
                        _isMerging = true;
                        SoundManager._Instance._PlaySound(GameSounds.Merge);
                        Vector3 newPos;
                        Vector2 rbVelocity;
                        float angVelocity;
                        if (transform.position.y < c2d.transform.position.y)
                        {
                            newPos = transform.position;
                            rbVelocity = _rigidbody.linearVelocity;
                            angVelocity = _rigidbody.angularVelocity;
                        }
                        else
                        {
                            newPos = c2d.gameObject.transform.position;
                            rbVelocity = c2d.GetComponent<Rigidbody2D>().linearVelocity;
                            angVelocity = c2d.GetComponent<Rigidbody2D>().angularVelocity;
                        }

                        PixelInfo newPixel = A.Pixels.NextPixel(_mergerInfo);
                        GameObject newGo = null;
                        if (newPixel != null)
                        {
                            newGo =
                            Instantiate(A.Pixels.PixelSkeleton
                            , newPos
                            , Quaternion.identity);
                            newGo.GetComponent<MergersController>()._LoadPixel(newPixel, rbVelocity, angVelocity, true);
                        }
                        OnMerge?.Invoke(c2d.gameObject, gameObject, newGo);
                        Destroy(c2d.gameObject);
                        Destroy(gameObject);

                    }
                }
            }
        }

    }
    private void OnMouseUp()
    {
        OnPixelClicked?.Invoke(gameObject);
    }

}
