window.HttpGet = async function (urlAddress, object = null) {
    let token = localStorage.getItem("authToken");
    console.log(token);
    //, "Authorization": "Bearer " + token
    let formData = "";
    let url = urlAddress;
    console.log(jwt_decode(token));
    console.log(urlAddress);

    if (object !== "" && object !== null && object !== undefined) {
        if (!object.noData) {
            this.console.log(object);
            Object.keys(object).forEach(function(key, index) {
                console.log(key);
                console.log(object[key]);
                formData = formData + key + "=" + object[key] + "&";
            });
            if (urlAddress.includes('?')) {
                url = url + '&' + formData;
            } else {
                url = url + '?' + formData;
            }
            url = url.slice(0, -1);
            this.console.log(formData);
            this.console.log(url);
        }
    }

    const response = await fetch(url, {
        method: "GET",
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