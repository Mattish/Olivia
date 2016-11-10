export default function(){
    return <ng.IDirective>{
        template: require<string>('./taketwo.template.html'),
        restrict: "E",
        controller: "TakeTwoController",
        controllerAs: "vm"
    }
}