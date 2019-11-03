window.CloseModalBackground = () => {
    var element = document.getElementsByClassName("modal-backdrop");
    element[0].parentNode.removeChild(element[0]);
};

window.CheckIsUserAdmin = () => {

    let token = localStorage.getItem("authToken");
    var decodedToken = jwt_decode(token);

    if (decodedToken.rol_Admin !== undefined) {
        return JSON.parse(decodedToken.rol_Admin) ? true : false;
    } else {
        return false;
    }
};

window.CheckIsUser = () => {

    let token = localStorage.getItem("authToken");
    var decodedToken = jwt_decode(token);

    if (decodedToken.rol_User !== undefined) {
        return JSON.parse(decodedToken.rol_User) ? true : false;
    } else {
        return false;
    }
};

window.CheckIsUserEditor = () => {

    let token = localStorage.getItem("authToken");
    var decodedToken = jwt_decode(token);

    if (decodedToken.rol_Editor !== undefined) {
        return JSON.parse(decodedToken.rol_Editor) ? true : false;
    } else {
        return false;
    }
};

window.UserApproved = () => {
    let isApproved = localStorage.getItem("lunchBreakApprovedUser") === 'true';
    return isApproved;
};

window.LogOut = () => {
    window.location.reload();
};

window.GoTo = (elementId) => {
    document.querySelector('#'+elementId).scrollIntoView({
        behavior: 'smooth'
    });
};