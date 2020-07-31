from django.urls import include, path
from rest_framework import routers
from CurrencyAPI.currency_converter import views

router = routers.DefaultRouter()
router.register(r'currency_converter', views.CurrencyViewSet)

urlpatterns = [
    path('', include(router.urls)),
    path('api-auth/', include('rest_framework.urls', namespace='rest_framework'))
]