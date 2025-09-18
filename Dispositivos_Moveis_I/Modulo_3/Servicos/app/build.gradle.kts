import java.util.Properties

val localProperties = Properties() // Use val for read-only properties
val localPropertiesFile = rootProject.file("local.properties")

if (localPropertiesFile.exists()) {
    localPropertiesFile.reader().use { reader -> // Use .reader() and .use for resource management
        localProperties.load(reader)
    }
}

plugins {
    alias(libs.plugins.android.application)
}

android {
    namespace = "com.example.servicos"
    compileSdk = 36

    defaultConfig {
        applicationId = "com.example.servicos"
        minSdk = 24
        targetSdk = 36
        versionCode = 1
        versionName = "1.0"

        val apiBaseUrl = localProperties.getProperty("API_BASE_URL")
        val apiTokenCep = localProperties.getProperty("API_TOKEN_CEP")
        val apiTokenCurrency = localProperties.getProperty("API_TOKEN_CURRENCY")

        buildConfigField("String", "API_BASE_URL", "\"$apiBaseUrl\"")
        buildConfigField("String", "API_TOKEN_CEP", "\"$apiTokenCep\"")
        buildConfigField("String", "API_TOKEN_CURRENCY", "\"$apiTokenCurrency\"")

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_11
        targetCompatibility = JavaVersion.VERSION_11
    }
    buildFeatures{
        buildConfig = true
    }
}

dependencies {

    implementation(libs.appcompat)
    implementation(libs.material)
    implementation(libs.activity)
    implementation(libs.constraintlayout)
    testImplementation(libs.junit)
    androidTestImplementation(libs.ext.junit)
    androidTestImplementation(libs.espresso.core)

    //Instalação de bibliotecas
    implementation("com.squareup.retrofit2:retrofit:2.11.0")
    implementation("com.squareup.retrofit2:converter-gson:2.11.0")
}