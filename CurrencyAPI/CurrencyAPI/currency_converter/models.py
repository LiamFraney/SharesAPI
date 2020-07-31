from django.db import models

class Currency(models.Model):
    symbol = models.CharField(max_length = 3, primary_key = True)
    conversion = models.FloatField()
    last_update = models.DateTimeField()
