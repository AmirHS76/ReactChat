export class userRepository {
    getUser = (userID: number) =>  {
        HttpService.Get("Users/id=" + userID);
    }

    getUsers = () => {
        HttpService.Get("Users");
    }


    updateUser = (data : object) => {
        HttpService.post("Users" , data);
    }
}