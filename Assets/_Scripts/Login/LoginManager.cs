using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : Singleton<LoginManager>
{
    [SerializeField]
    private TMP_InputField _accountField;
    [SerializeField]
    private TMP_InputField _passwordField;
    [SerializeField]
    private Button _loginPanelButton = null;
    [SerializeField]
    private Button _registerPanelButton = null;
    [SerializeField]
    private Button _loginButton = null;
    [SerializeField]
    private Button _registerButton = null;
    [SerializeField]
    private Button _cancelButton = null;
    [Space]
    [SerializeField]
    private GameObject _panelLoginFail = null;
    [SerializeField]
    private TextMeshProUGUI _text = null;
    [SerializeField]
    private GameObject _panelLoading = null;

    [SerializeField] 
    private GameObject _appTitle = null;
    [SerializeField] 
    private GameObject _orText = null;

    protected override void Awake()
    {
        base.Awake();
        _accountField.gameObject.SetActive(false);
        _passwordField.gameObject.SetActive(false);
        _loginButton.gameObject.SetActive(false);
        _registerButton.gameObject.SetActive(false);
        _loginPanelButton.gameObject.SetActive(true);
        _registerPanelButton.gameObject.SetActive(true);
        _panelLoginFail.SetActive(false);
        _cancelButton.gameObject.SetActive(false);
        _panelLoading.SetActive(false);
        _appTitle.SetActive(true);
        _orText.SetActive(true);
    }
    public void OnClickShowPanelLogin()
    {
        _accountField.gameObject.SetActive(true);
        _passwordField.gameObject.SetActive(true);
        _loginPanelButton.gameObject.SetActive(false);
        _registerPanelButton.gameObject.SetActive(false);
        _loginButton.gameObject.SetActive(true);
        _registerButton.gameObject.SetActive(false);
        _cancelButton.gameObject.SetActive(true);
        _appTitle.SetActive(false);
        _orText.SetActive(false);
    }
    public void OnClickShowPanelRegister()
    {
        _accountField.gameObject.SetActive(true);
        _passwordField.gameObject.SetActive(true);
        _loginPanelButton.gameObject.SetActive(false);
        _registerPanelButton.gameObject.SetActive(false);
        _loginButton.gameObject.SetActive(false);
        _registerButton.gameObject.SetActive(true);
        _cancelButton.gameObject.SetActive(true);
        _appTitle.SetActive(false);
        _orText.SetActive(false);
    }
    public void OnClickLoginButton()
    {
        StartCoroutine(OnLogin());
        //SceneManager.Instance.ChangeScene(Define.SceneName.Main.ToString(), null);
    }
    private IEnumerator OnLogin()
    {
        _panelLoading.SetActive(true);
        yield return ResourceManager.Instance.RequestLogin(_accountField.text, _passwordField.text);
        if (!ResourceManager.Instance.CanLogin)
        {
            _panelLoading.SetActive(false);
            _panelLoginFail.SetActive(true);
            _text.text = "Login Fail!";
            yield return new WaitForSeconds(3);
            _panelLoginFail.SetActive(false);
        }
    }
    public void OnClickRegisterButton()
    {
        StartCoroutine(OnRegister());
    }
    private IEnumerator OnRegister()
    {
        yield return ResourceManager.Instance.RequestRegister(_accountField.text, _passwordField.text);
        if (!ResourceManager.Instance.CanRegister)
        {
            _panelLoginFail.SetActive(true);
            _text.text = "Login Fail!";
            yield return new WaitForSeconds(3);
            _panelLoginFail.SetActive(false);
        }
    }
    public void OnClickCancelButton()
    {
        _accountField.gameObject.SetActive(false);
        _passwordField.gameObject.SetActive(false);
        _loginButton.gameObject.SetActive(false);
        _registerButton.gameObject.SetActive(false);
        _loginPanelButton.gameObject.SetActive(true);
        _registerPanelButton.gameObject.SetActive(true);
        _panelLoginFail.SetActive(false);
        _cancelButton.gameObject.SetActive(false);
        _appTitle.SetActive(true);
        _orText.SetActive(true);
    }
}