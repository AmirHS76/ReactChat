
import axios from 'axios';
export class Http {
    post = (url: string, data : object) => {

    }


    get = (url: string) => {

        axios.get(localStorage.BaseUrl + url, {
            headers: { Authorization: `Bearer ${localStorage.AuthToken}` }
        });
    }

    delete = (url: string) => { }

    update = (url: string , data:object) => {

    }
}