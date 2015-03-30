angular.module('QYSendModalController', ['WXService', 'WXDirective', 'ui.bootstrap']).controller('sendModalCtrl', ['$scope', '$http', 'instance', 'memberService', '$modalInstance', 'sendTargetList', 'sendTargetText', function ($scope, $http, instance, memberService, $modalInstance, sendTargetList, sendTargetText) {
    $scope.urlMenu = '/QYDepart/GetDepartMenu';

    $scope.sendTargetList =[];
    $scope.sendTargetItem = {};
    $scope.sendTargetText = '';

    if (sendTargetList != undefined && sendTargetText != undefined)
    {
        $scope.sendTargetList = sendTargetList;
        $scope.sendTargetText = sendTargetText;
    }

    //獲取成員列表
    $scope.getMemberList = function (depPKID) {
       memberService.getMemberList(depPKID).success(function(data){
           $scope.memberList = data;

           for (var i = 0; i < $scope.sendTargetList.length; i++)
           {
               if($scope.sendTargetList[i].type!='1')
                   continue;
               for (var j = 0; j < $scope.memberList.length; j++)
               {
                   if ($scope.memberList[j].Name == $scope.sendTargetList[i].Name && $scope.memberList[j].UserId == $scope.sendTargetList[i].id) {
                       $scope.memberList[j].IsChecked = true;
                       break;
                   }
               }
           }

        }).error(function(){
            alert('獲取成員失敗');
        });
    }


    //成员checkbox事件
    $scope.memberToggleChecked = function (item) {
        if (!item.IsChecked) {
            $scope.sendTargetItem = {};
            item.IsChecked = true;
            $scope.sendTargetItem.type = '1';
            $scope.sendTargetItem.id = item.UserId;
            $scope.sendTargetItem.Name = item.Name;
            $scope.sendTargetList.push($scope.sendTargetItem);
            
        }
        else {
            item.IsChecked = false;
            for (var i = 0; i < $scope.sendTargetList.length; i++)
            {
                if ($scope.sendTargetList[i].type == '1' && $scope.sendTargetList[i].id==item.UserId) {
                    $scope.sendTargetList.splice(i, 1);
                    i--;
                    break;
                }
            }
        }

        $scope.getSendTargetNames($scope.sendTargetList);

    }

    //显示发送对象名字
    $scope.getSendTargetNames = function (sendTargetList) {
        $scope.sendTargetText = '';
        for (var i = 0; i < sendTargetList.length; i++)
        {
            $scope.sendTargetText += sendTargetList[i].Name + '|';
        }
        $scope.sendTargetText = $scope.sendTargetText.substring(0, $scope.sendTargetText.length-1);
    }

    //将发送对象传回父作用域
    $scope.sendTargetOK = function () {
        $modalInstance.close($scope.sendTargetList);
    }

    //关闭modal框
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    }
}
]);