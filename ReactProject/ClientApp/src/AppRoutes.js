import CreateUser from "./components/CreateUser"
import MainPage from "./components/MainPage"
import TwitterProfile from "./components/TwitterProfile"

const AppRoutes = [
    {
        index: true,
        element: <MainPage />

  },
    {
        path: 'profile/:id',
        element: <TwitterProfile />
    },

    {
        path: 'CreateAccount',
        element: <CreateUser />
    }

];

export default AppRoutes;
