window.HttpGet = async function (urlAddress, urlData) {
    let token = localStorage.getItem("authToken");
    console.log(token);
    //, "Authorization": "Bearer " + token
    
    console.log(jwt_decode(token));

    const response = await fetch(urlAddress, {
        method: "GET",
        cache: "no-cache",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + JSON.parse(token)
            // "Content-Type": "application/x-www-form-urlencoded",
        }
        //body: JSON.stringify(object)
    }).then(function (data) {
        console.log(data);
        return data.json();
        // Here you get the data to modify as you please
    }).then(function (data) {
        console.log(data);
        return data;
    }).catch(function (error) {
        console.log(error);
        // If there is any error you will catch them here
    });
    console.log(response);
    return response;
};

window.HttpPost = async function (urlAddress, object) {
    let token = localStorage.getItem("authToken");
    console.log(token);
    const response = await fetch(urlAddress, {
        method: "POST",
        cache: "no-cache",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + JSON.parse(token)
            // "Content-Type": "application/x-www-form-urlencoded",
        },
        body : JSON.stringify(object)
    }).then(function (data) {
        console.log(data);
        return data.json();
        // Here you get the data to modify as you please
    }).then(function(data) {
        console.log(data);
        return data;
    }).catch(function (error) {
        console.log(error);
        // If there is any error you will catch them here
    });
    console.log(response);
    return response;
};

window.HttpDelete = async function (urlAddress) {
    let token = localStorage.getItem("authToken");
    console.log(token);
    const response = await fetch(urlAddress, {
        method: "DELETE",
        cache: "no-cache",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + JSON.parse(token)
            // "Content-Type": "application/x-www-form-urlencoded",
        }
    }).then(function (data) {
        console.log(data);
        return data.json();
        // Here you get the data to modify as you please
    }).then(function (data) {
        console.log(data);
        return data;
    }).catch(function (error) {
        console.log(error);
        // If there is any error you will catch them here
    });
    console.log(response);
    return response;
};

window.HttpPut = async function (urlAddress, object) {
    let token = localStorage.getItem("authToken");
    console.log(token);
    const response = await fetch(urlAddress, {
        method: "PUT",
        cache: "no-cache",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + JSON.parse(token)
            // "Content-Type": "application/x-www-form-urlencoded",
        },
        body: JSON.stringify(object)
    }).then(function (data) {
        console.log(data);
        return data.json();
        // Here you get the data to modify as you please
    }).then(function (data) {
        console.log(data);
        return data;
    }).catch(function (error) {
        console.log(error);
        // If there is any error you will catch them here
    });
    console.log(response);
    return response;
};