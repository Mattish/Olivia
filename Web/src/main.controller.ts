interface IMainControllerScope extends ng.IScope{
    Name: string;
}

export default class MainController{

    static $inject = ['$scope'];
    constructor($scope: IMainControllerScope){
        $scope.Name = "Matthew";
    }
}