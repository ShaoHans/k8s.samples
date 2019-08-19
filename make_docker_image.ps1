<#
请务必定位到src根目录执行该脚本，ver版本参数不能为空，如果忘记当前生产环境运行的镜像版本，可登录阿里云镜像服务查看，然后在当前版本值上递增
例如：.\Fnlinker.Pm.API\make_docker_image.ps1 -ver 1.0
#>
param(
[string]$ver=$(throw "镜像版本参数不能为空") 
)

docker build -t k8s.samples.user.api --build-arg environment=Production .
docker tag k8s.samples.user.api registry.cn-hangzhou.aliyuncs.com/shz/k8s.samples.user.api:$ver
docker push registry.cn-hangzhou.aliyuncs.com/shz/k8s.samples.user.api:$ver