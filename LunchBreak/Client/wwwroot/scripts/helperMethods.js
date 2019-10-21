window.CloseModalBackground = () => {
    var element = document.getElementsByClassName("modal-backdrop");
    element[0].parentNode.removeChild(element[0]);
};

window.CheckIsUserAdmin = () => {

    let token = localStorage.getItem("authToken");
    var decodedToken = jwt_decode(token);
    console.log(jwt_decode(token));
    console.log(decodedToken.rol_Admin);
    return JSON.parse(decodedToken.rol_Admin);
};

window.CheckIsUser = () => {

    let token = localStorage.getItem("authToken");
    var decodedToken = jwt_decode(token);
    console.log(jwt_decode(token));
    console.log(decodedToken.rol_User);
    return JSON.parse(decodedToken.rol_User);
};

window.CheckIsUserEditor = () => {

    let token = localStorage.getItem("authToken");
    var decodedToken = jwt_decode(token);
    console.log(jwt_decode(token));
    console.log(decodedToken.rol_Editor);
    return JSON.parse(decodedToken.rol_Editor);
};

window.LogOut = () => {
    window.location.reload();
};